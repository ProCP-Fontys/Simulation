
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntryInformation;

    [Serializable]
public class CrossingB : Crossing
{
    public CrossingB(int crossingID, Simulation simulation)
        : base(crossingID, simulation) { }
}

