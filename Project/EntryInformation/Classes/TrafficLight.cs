﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TrafficLight
{
	public virtual int GreenLight
	{
		get;
		set;
	}

	public virtual bool State
	{
		get;
		set;
	}

	public virtual int Time
	{
		get;
		set;
	}

	public virtual void SetState()
	{
		throw new System.NotImplementedException();
	}

	public virtual void GetState()
	{
		throw new System.NotImplementedException();
	}

}
