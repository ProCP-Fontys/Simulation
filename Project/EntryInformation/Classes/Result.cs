
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class Result
{
	public virtual List <Car> Cars
	{
		get;
		set;
	}

	public virtual double AvgTimeExpend
	{
		get;
		set;
	}

	public virtual double MinTimeExpend
	{
		get;
		set;
	}

	public virtual double MaxTimeExpend
	{
		get;
		set;
	}

	public Car cars
	{
		get;
		set;
	}

	public virtual void CalculateMaxTime()
	{
		throw new System.NotImplementedException();
	}

	public virtual void CalculateMinTime()
	{
		throw new System.NotImplementedException();
	}

	public virtual void CalculateAvgTime()
	{
		throw new System.NotImplementedException();
	}

}

