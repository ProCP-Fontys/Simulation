
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntryInformation;

[Serializable]
public abstract class Crossing 
{
    public int CrossingID { get; set; }
    public List<Feeder> Feeders { get; set; }
    public Neighbours neighbors { get; set; }
    public int FeederIDIndexToRestart { get; set; }
    [NonSerialized]
    private Simulation simulation;

    public Crossing(int crossingID, Simulation simulation)
    {
        this.simulation = simulation;
        this.CrossingID = crossingID;
        Feeders = new List<Feeder>();
        neighbors = new Neighbours();

        for (int i = 1; i < 5; i++)
        {
            Feeders.Add(new Feeder(i,this,simulation));
        }
    }

	public virtual int GridCellNumber
	{
		get;
		set;
	}

	public virtual List<Timers> TimerList
	{
		get;
		set;
	}

	public virtual IEnumerable<Feeder> Feeder
	{
		get;
		set;
	}

    public virtual void AddDetailes(int feederID, Crossing crossing, int rPercentage, int lPercentage, int sPercentage, int carQuantity, int pedTimer)
	{
		
	}

}

