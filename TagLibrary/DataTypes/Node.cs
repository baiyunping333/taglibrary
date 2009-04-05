using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace TagLibrary.DataTypes
{
    class Node
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

        private int parentId;

        public int ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

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



    }
}
