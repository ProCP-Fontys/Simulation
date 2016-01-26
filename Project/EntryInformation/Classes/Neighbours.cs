
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class Neighbours
{
	public Crossing Left
	{
		get;
		set;
	}

	public Crossing Right
	{
		get;
		set;
	}

	public Crossing Top
	{
		get;
		set;
	}

	public Crossing Bottom
	{
		get;
		set;
	}

	public List<Crossing> RetrieveListOfNeighbors()
	{
        return new List<Crossing>() { Left, Top, Right, Bottom };
	}

}

