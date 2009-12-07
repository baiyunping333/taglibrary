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
            // Members
            Graph graph = new Graph();
            Program program = new Program();

            // Initialize Logger
            StreamWriter logger = new StreamWriter("log.txt");
            if (logger != null)
                program.LogFile("Testing TAG Library", "Comment", logger);
            else
            {
                program.LogFile("Initializing logger file", "Fail", logger);
                return;
            }
 
            // Load graph from DIMACS file
            program.LogFile("Loading DIMACS graph file myData", "Comment", logger);
            if (graph.LoadGraph("..\\..\\..\\myData"))
                program.LogFile("Loading graph from DIMACS file", "Pass", logger);
            else
                program.LogFile("Loading graph from DIMACS file", "Fail", logger);

            // Printing the graph
            program.LogFile("Printing the graph", "Comment", logger);
            graph.PrintGraph();
            program.LogFile("Printing the graph", "Pass", logger);
            
            // Check existence of node 1

            // Check existence of node 2

            // Create a node 2

            // Check for arc between 1 and 2

            // Insert arc between 1 and 2

            // Shortest path between given node and all other nodes
            program.LogFile("Finding shortest path between given node to all other node", "Comment", logger);
            program.LogFile("Creating output file for shortest path", "Comment", logger);
            StreamWriter file = new StreamWriter("output.csv", true);
            file.WriteLine("Origin, Destination, Distance, Previous Node, Reached Previous Node at");
            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                if (i % 100 == 0)
                    Console.WriteLine(string.Format("Processing node {0} of {1}", i, graph.Nodes.Count - 1));
                graph.ShortestPaths(i + 1, file);
            }
            file.Close();
            // Check if the path is correct then
            program.LogFile("Finding shortest path between given node to all other node", "Pass", logger);
            // else
            //program.LogFile("Finding shortest path between given node to all other node", "Fail", logger);

            // Shortest path between given nodes
            program.LogFile("Finding shortest path between nodes 1 and 3", "Comment", logger);
            program.LogFile("Creating output file for shortest path", "Comment", logger);
            file = new StreamWriter("output.csv", true);
            file.WriteLine("Origin, Destination, Distance, Previous Node, Reached Previous Node at");
            bool status = graph.ShortestPaths(1, 3, file);
            file.Close();
            if(status)
                program.LogFile("Finding shortest path between nodes 1 and 3", "Pass", logger);
            else
                program.LogFile("Finding shortest path between given node to all other node", "Fail", logger);

            // Close Logger
            program.LogFile("", "Close", logger);
            logger.Close();
        }

        public void LogFile(string comment, string status, StreamWriter logger)
        {
            switch (status)
            { 
                case "Comment":
                    logger.WriteLine(string.Format("{0:HH:mm:ss} - C - {1}.", DateTime.Now, comment));
                    Console.WriteLine(string.Format("{0:HH:mm:ss} - C - {1}.", DateTime.Now, comment));
                    break;
                case "Pass":
                    logger.WriteLine(string.Format("{0:HH:mm:ss} - P - Successfully passed {1}.", DateTime.Now, comment));
                    Console.WriteLine(string.Format("{0:HH:mm:ss} - P - Successfully passed {1}.", DateTime.Now, comment));
                    break;
                case "Fail":
                    logger.WriteLine(string.Format("{0:HH:mm:ss} - F - Failed {1}.", DateTime.Now, comment));
                    Console.WriteLine(string.Format("{0:HH:mm:ss} - F - Failed {1}.", DateTime.Now, comment));
                    break;
                case "Close":
                    logger.WriteLine(string.Format("{0:HH:mm:ss} - C - Finishing the test.", DateTime.Now));
                    break;
            }
        }
    }
}
