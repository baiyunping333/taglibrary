using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace TagLibrary.DataTypes
{
    public class Node
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private int intialOcuupancy;

        public int IntialOcuupancy
        {
            get { return intialOcuupancy; }
            set { intialOcuupancy = value; }
        }

        private int numberOfNeighbours;

        public int NumberOfNeighbours
        {
            get { return numberOfNeighbours; }
            set { numberOfNeighbours = value; }
        }

        // 20091110 - Santhosh - Have to check!
        private int parentId;

        public int ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        // 20091110 - Santhosh - Have to check!
        private int parentTime;

        public int ParentTime
        {
            get { return parentTime; }
            set { parentTime = value; }
        }

        private List<Arc> arcs;

        public List<Arc> Arcs
        {
            get { return arcs; }
            set { arcs = value; }
        }

        public Node()
        {
            arcs = new List<Arc>();
        }

        public bool addArc(Arc a)
        {
            // check if the arc exists already
            if( ! arcs.Exists(item => item.EndNode == a.EndNode))
            {
                Arcs.Add(a);
                NumberOfNeighbours++;
                return true;
            }
            else return false;
        }

        public bool removeArc(Arc a)
        {
            // check if the arc already
            if (arcs.Exists(item => item.EndNode == a.EndNode))
            {
                arcs.Remove(a);
                NumberOfNeighbours--;
                return true;
            }
            else return false;
        }

    }
}
