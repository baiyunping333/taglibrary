using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace TagLibrary.DataTypes
{
    class Arc
    {
        //TODO: Verify
        private int endNode;

        public int EndNode
        {
            get { return endNode; }
            set { endNode = value; }
        }
        //TODO: Verify
        private Arc arcNext;

        internal Arc ArcNext
        {
            get { return arcNext; }
            set { arcNext = value; }
        }

        private List<int> travelTimeSeries;

        public List<int> TravelTimeSeries
        {
            get { return travelTimeSeries; }
            set { travelTimeSeries = value; }
        }
        private List<int> bestTravelTimeSeries;

        public List<int> BestTravelTimeSeries
        {
            get { return bestTravelTimeSeries; }
            set { bestTravelTimeSeries = value; }
        }
    }
}
